using System;
using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;
using strange.extensions.reflector.api;
using strange.extensions.reflector.impl;

namespace strange.unittests
{
	[TestFixture()]
	public class TestReflectionBinder
	{
		IReflectionBinder reflector;

		[SetUp]
		public void SetUp()
		{
			reflector = new ReflectionBinder ();
		}

		[Test]
		public void TestFoundSoleConstructor ()
		{

		}

		[Test]
		public void TestConstructorWithNoParameters ()
		{
			IReflectedClass reflected = reflector.Get<ClassToBeInjected> ();
			Assert.AreEqual (0, reflected.constructorParameters.Length);
		}

		[Test]
		public void TestConstructorWithSingleParameter ()
		{
			IReflectedClass reflected = reflector.Get<ClassWithConstructorParametersOnlyOneConstructor> ();
			Assert.AreEqual (1, reflected.constructorParameters.Length);
		}

		[Test]
		public void TestTaggedConstructor ()
		{
			IReflectedClass reflected = reflector.Get<ClassWithConstructorParameters> ();
			Assert.AreEqual (2, reflected.constructorParameters.Length);

			ConstructorInfo constructor = reflected.constructor;
			object[] parameters = new object[2];
			parameters [0] = 42;
			parameters [1] = "Zaphod";
			ClassWithConstructorParameters instance = constructor.Invoke(parameters) as ClassWithConstructorParameters;
			Assert.IsNotNull (instance);
			Assert.AreEqual (42, instance.intValue);
			Assert.AreEqual ("Zaphod", instance.stringValue);
		}

		[Test]
		public void TestShortestConstructor ()
		{
			IReflectedClass reflected = reflector.Get<MultipleConstructorsUntagged> ();
			Assert.AreEqual (3, reflected.constructorParameters.Length);

			ISimpleInterface simple = new SimpleInterfaceImplementer ();
			simple.intValue = 11001001;

			ConstructorInfo constructor = reflected.constructor;
			object[] parameters = new object[3];
			parameters [0] = simple;
			parameters [1] = 42;
			parameters [2] = "Zaphod";
			MultipleConstructorsUntagged instance = constructor.Invoke(parameters) as MultipleConstructorsUntagged;
			Assert.IsNotNull (instance);
			Assert.AreEqual (simple.intValue, instance.simple.intValue);
			Assert.AreEqual (42, instance.intValue);
			Assert.AreEqual ("Zaphod", instance.stringValue);
		}

		[Test]
		public void TestConstructorWithMultipleParameters ()
		{
			IReflectedClass reflected = reflector.Get<ClassWithConstructorParameters> ();
			Assert.AreEqual (2, reflected.constructorParameters.Length);
		}

		[Test]
		public void TestSinglePostConstruct ()
		{
			IReflectedClass reflected = reflector.Get<PostConstructClass> ();
			Assert.AreEqual (1, reflected.postConstructors.Length);
		}

		[Test]
		public void TestMultiplePostConstructs ()
		{
			IReflectedClass reflected = reflector.Get<PostConstructTwo> ();
			Assert.AreEqual (2, reflected.postConstructors.Length);
		}

		[Test]
		public void TestSingleSetter ()
		{
			IReflectedClass reflected = reflector.Get<PostConstructTwo> ();
			Assert.AreEqual (1, reflected.setters.Length);
			Assert.AreEqual (1, reflected.setterNames.Length);
			Assert.IsNull (reflected.setterNames[0]);

			KeyValuePair<Type, PropertyInfo> pair = reflected.setters [0];
			Assert.AreEqual (pair.Key, typeof(float));
		}

		[Test]
		public void TestMultipleSetters ()
		{
			IReflectedClass reflected = reflector.Get<HasTwoInjections> ();
			Assert.AreEqual (2, reflected.setters.Length);
			Assert.AreEqual (2, reflected.setterNames.Length);
			Assert.IsNull (reflected.setterNames[0]);

			bool foundStringType = false;
			bool foundInjectableSuperClassType = false;

			foreach (KeyValuePair<Type, PropertyInfo> pair in reflected.setters)
			{
				if (pair.Key == typeof(string))
				{
					foundStringType = true;
					Assert.AreEqual ("injectionTwo", pair.Value.Name);
				}
				if (pair.Key == typeof(InjectableSuperClass))
				{
					foundInjectableSuperClassType = true;
					Assert.AreEqual ("injectionOne", pair.Value.Name);
				}
			}
			Assert.True (foundStringType);
			Assert.True (foundInjectableSuperClassType);
		}

		[Test]
		public void TestNamedSetters ()
		{
			IReflectedClass reflected = reflector.Get<HasNamedInjections> ();
			Assert.AreEqual (2, reflected.setters.Length);
			Assert.AreEqual (2, reflected.setterNames.Length);

			int a = 0;
			int injectableSuperClassCount = 0;
			bool foundSomeEnum = false;
			bool foundMarkerClass = false;

			foreach (KeyValuePair<Type, PropertyInfo> pair in reflected.setters)
			{
				if (pair.Key == typeof(InjectableSuperClass))
				{
					injectableSuperClassCount ++;

					object name = reflected.setterNames [a];
					if (name.Equals(SomeEnum.ONE))
					{
						Assert.False (foundSomeEnum);
						foundSomeEnum = true;
					}
					if (name.Equals(typeof(MarkerClass)))
					{
						Assert.False (foundMarkerClass);
						foundMarkerClass = true;
					}
				}
				a++;
			}

			Assert.AreEqual (2, injectableSuperClassCount);
		}

		[Test]
		public void TestSettersOnDerivedClass()
		{
			IReflectedClass reflected = reflector.Get<InjectableDerivedClass> ();
			Assert.AreEqual (2, reflected.setters.Length);
			Assert.AreEqual (2, reflected.setterNames.Length);

			bool foundIntType = false;
			bool foundClassToBeInjectedType = false;

			foreach (KeyValuePair<Type, PropertyInfo> pair in reflected.setters)
			{
				if (pair.Key == typeof(int))
				{
					foundIntType = true;
					Assert.AreEqual ("intValue", pair.Value.Name);
				}
				if (pair.Key == typeof(ClassToBeInjected))
				{
					foundClassToBeInjectedType = true;
					Assert.AreEqual ("injected", pair.Value.Name);
				}
			}
			Assert.True (foundIntType);
			Assert.True (foundClassToBeInjectedType);
		}

		[Test]
		public void TestReflectAnInterface ()
		{
			TestDelegate testDelegate = delegate()
			{
				reflector.Get<ISimpleInterface> ();
			};
			ReflectionException ex = Assert.Throws<ReflectionException>(testDelegate);
			Assert.That (ex.type == ReflectionExceptionType.CANNOT_REFLECT_INTERFACE);
		}

		[Test]
		public void TestReflectionCaching()
		{
			IReflectedClass reflected = reflector.Get<HasNamedInjections> ();
			Assert.False (reflected.preGenerated);
			IReflectedClass reflected2 = reflector.Get<HasNamedInjections> ();
			Assert.True (reflected2.preGenerated);
		}
	}
}

