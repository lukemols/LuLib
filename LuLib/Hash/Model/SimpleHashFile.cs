using System;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace LuLib.Hash.Model
{
	public enum HashCalculationStatus { NotComputed, InProgress, Computed }

	public class SimpleHashFile : IComparable<SimpleHashFile>, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		string name;
		public string Name { get { return name; } }

		string path;
		public string Path { get { return path; } }

		byte[] hash;

		public byte[] Hash { get { return hash; } }

		public string HashCode { get { return BitConverter.ToString(hash).Replace("-", string.Empty); } }

		public HashCalculationStatus Status { get; private set; }

		int HashLength { get => 20; }

		public SimpleHashFile(string FilePath)
		{
			path = FilePath;
			name = System.IO.Path.GetFileName(FilePath);
			hash = new byte[HashLength];
			Status = HashCalculationStatus.NotComputed;
		}

		public bool FileExists()
		{
			return File.Exists(path);
		}

		public async Task ComputeFileHash()
		{
			if (!FileExists())
			{
				return;
			}

			Status = HashCalculationStatus.InProgress;
			await Task.Run(() =>
			{
				FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
				int b, i = 0;
				long count = 0;
				while ((b = fs.ReadByte()) != -1)
				{
					hash[i] = (byte)(hash[i] ^ System.Convert.ToByte(b));
					i = (i + 1) % HashLength;
					count++;
				}

				Status = HashCalculationStatus.Computed;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HashCalculated"));
			});

		}

		/// <summary>
		/// Compare two SimpleHashFiles from their Hash Codes.
		/// </summary>
		/// <param name="other">Other hash file.</param>
		/// <returns>-1 if is minor, +1 if is major, 0 if they are equal.</returns>
		public int CompareTo(SimpleHashFile other)
		{
			for (int i = 0; i < hash.Length; i++)
			{
				if (hash[i] < other.Hash[i])
					return -1;
				else if (hash[i] > other.Hash[i])
					return 1;
			}
			return 0;
		}
	}
}
